// Usage: <hd-comments model="<model_name>" category="<title>" load-more="<fn>"></hd-comments>

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

  function CommentsDirective($window, $modal, User, Comment) {
    return {
      restrict: 'E',
      replace: true,
      templateUrl: 'directives/comments/commentsDirective.html',
      scope: {
        category: '@',
        model: '=',
        loadMore: '&',
        userImg: '='
      },
      link: function ($scope) {
        var skip = 20;

        $scope.pending = false;
        $scope.userId = $window.user.userId;

        $scope.loadMore = function () {
          if (!$scope.pending) {
            $scope.pending = true;
            return User.userComments.query({ userId: $scope.userId, skip: skip })
                .$promise.then(function (comments) {
              $scope.model = $scope.model.concat(comments);
              skip += 20;
              $scope.pending = false;

              return comments.length !== 0;
            });
          }
        };

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

  CommentsDirective.$inject = ['$window', '$modal', 'User', 'Comment'];

  angular.module('directives').directive('hdComments', CommentsDirective);
}(angular));