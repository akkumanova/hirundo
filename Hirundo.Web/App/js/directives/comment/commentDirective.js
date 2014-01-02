// Usage: <hd-comments model="<model_name>"></hd-comments>

/*global angular*/
(function (angular) {
  'use strict';

  var ModalCtrl = function ($scope, $modalInstance, commentData, modalData) {
    $scope.commentData = commentData;
    $scope.header = modalData.header;
    $scope.action = modalData.action;

    $scope.cancel = function () {
      $modalInstance.close(false);
    };

    $scope.ok = function () {
      $modalInstance.close(true);
    };
  };

  function CommentDirective($window, $modal, $state, Comment) {
    function CommentLink($scope) {
      $scope.userId = $window.user.userId;

      $scope.sendReply = function () {
        var commentId = $scope.model.commentId;
        var reply = {
          'Author': $window.user.userId,
          'Content': $scope.model.newReply,
          'PublishDate': new Date()
        };

        return Comment.reply.save({ commentId: commentId }, reply).$promise.then(function () {
          Comment.commentDetails.get({ commentId: commentId }).$promise
                 .then(function (commentDetails) {
            $scope.model.replies = commentDetails.replies;
          });
        });
      };

      $scope.commentClick = function () {
        $scope.model.isExpanded = !$scope.model.isExpanded;

        if ($scope.model.isExpanded) {
          Comment.commentDetails.get({ commentId: $scope.model.commentId })
                 .$promise.then(function (commentDetails) {
            $scope.model.retweets = commentDetails.retweets;
            $scope.model.favorites = commentDetails.favorites;
            $scope.model.replies = commentDetails.replies;
          });
        }
      };

      $scope.retweetComment = function () {
        var modalInstance = $modal.open({
          templateUrl: 'directives/comment/modal.html',
          controller: ModalCtrl,
          windowClass: 'comment-modal',
          resolve: {
            commentData: function () {
              return {
                author: $scope.model.author,
                authorImg: $scope.model.authorImg,
                publishDate: $scope.model.publishDate,
                content: $scope.model.content
              };
            },
            modalData: function () {
              return {
                header: 'Retweet this to your followers?',
                action: 'Retweet'
              };
            }
          }
        });

        modalInstance.result.then(function (result) {
          if (result) {
            var commentId = $scope.model.commentId;

            Comment.retweet.save({ commentId: commentId }).$promise.then(function () {
              Comment.commentDetails.get({ commentId: commentId }).$promise
                      .then(function (commentDetails) {
                $scope.model.isRetweeted = true;
                $scope.model.retweets = commentDetails.retweets;
              });
            });
          }
        });
      };

      $scope.deleteComment = function () {
        var modalInstance = $modal.open({
          templateUrl: 'directives/comment/modal.html',
          controller: ModalCtrl,
          windowClass: 'comment-modal',
          resolve: {
            commentData: function () {
              return {
                author: $scope.model.author,
                authorImg: $scope.model.authorImg,
                publishDate: $scope.model.publishDate,
                content: $scope.model.content
              };
            },
            modalData: function () {
              return {
                header: 'Are you sure you want to delete this hirundo?',
                action: 'Delete'
              };
            }
          }
        });

        modalInstance.result.then(function (result) {
          if (result) {
            var commentId = $scope.model.commentId;

            Comment.comment['delete']({ commentId: commentId }).$promise.then(function () {
              $state.go('home', {}, { reload: true });
            });
          }
        });
      };
    }

    return {
      restrict: 'E',
      replace: true,
      templateUrl: 'directives/comment/commentDirective.html',
      scope: {
        model: '=',
        userImg: '='
      },
      link: CommentLink
    };
  }

  CommentDirective.$inject = ['$window', '$modal', '$state', 'Comment'];

  angular.module('directives').directive('hdComment', CommentDirective);
}(angular));