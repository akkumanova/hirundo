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
          templateUrl: 'directives/comment/retweetModal.html',
          controller: RetweetModalCtrl,
          windowClass: 'retweet-modal',
          resolve: {
            commentData: function () {
              return {
                author: $scope.model.author,
                authorImg: $scope.model.authorImg,
                publishDate: $scope.model.publishDate,
                content: $scope.model.content
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

  CommentDirective.$inject = ['$window', '$modal', 'Comment'];

  angular.module('directives').directive('hdComment', CommentDirective);
}(angular));