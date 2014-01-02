// Usage: <hd-comments model="<model_name>"></hd-comments>

/*global angular*/
(function (angular) {
  'use strict';

  var RetweetModalCtrl = function ($scope, $modalInstance, commentData) {
    $scope.commentData = commentData;

    $scope.cancel = function () {
      $modalInstance.close(false);
    };

    $scope.ok = function () {
      $modalInstance.close(true);
    };
  };

  function CommentDirective($window, $modal, Comment) {
    return {
      restrict: 'E',
      replace: true,
      templateUrl: 'directives/comment/commentDirective.html',
      scope: {
        model: '=',
        userImg: '='
      },
      link: function ($scope) {
        $scope.userId = $window.user.userId;

        $scope.sendReply = function (comment) {
          var reply = {
            'Author': $window.user.userId,
            'Content': comment.newReply,
            'PublishDate': new Date()
          };

          return Comment.reply.save({ commentId: comment.commentId }, reply)
                        .$promise.then(function () {
                          comment.replies.pop();
                          comment.replies.unshift({
                            author: $window.user.username,
                            authorId: reply.Author,
                            authorImg: $scope.userImg,
                            content: reply.Content,
                            publishDate: reply.PublishDate
                          });
                        });
        };

        $scope.commentClick = function (comment) {
          comment.isExpanded = !comment.isExpanded;

          if (comment.isExpanded) {
            Comment.commentDetails.get({ commentId: comment.commentId })
                   .$promise.then(function (commentDetails) {
              comment.retweets = commentDetails.retweets;
              comment.favorites = commentDetails.favorites;
              comment.replies = commentDetails.replies;
            });
          }
        };

        $scope.retweetComment = function (comment) {
          var modalInstance = $modal.open({
            templateUrl: 'directives/comments/retweetModal.html',
            controller: RetweetModalCtrl,
            windowClass: 'retweet-modal',
            resolve: {
              commentData: function () {
                return {
                  author: comment.author,
                  authorImg: comment.authorImg,
                  publishDate: comment.publishDate,
                  content: comment.content
                };
              }
            }
          });

          modalInstance.result.then(function (result) {
            if (result) {
              Comment.retweet.save({ commentId: comment.commentId }).$promise.then(function () {
                comment.isRetweeted = true;
                comment.retweets += 1;
              });
            }
          });
        };
      }
    };
  }

  CommentDirective.$inject = ['$window', '$modal', 'Comment'];

  angular.module('directives').directive('hdComment', CommentDirective);
}(angular));