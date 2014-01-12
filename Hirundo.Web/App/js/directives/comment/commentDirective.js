// Usage: <hd-comments model="<model_name>"></hd-comments>

/*global angular*/
(function (angular) {
  'use strict';

  function CommentDirective($window, Comment) {
    function CommentLink($scope) {
      $scope.userId = $window.user.userId;
      $scope.userImg = $window.user.userImage;

      $scope.sendReply = function () {
        var commentId = $scope.model.commentId;
        var reply = {
          'Author': $window.user.userId,
          'Content': $scope.model.newReply,
          'PublishDate': new Date()
        };

        return Comment.reply.save({ commentId: commentId }, reply).$promise
                            .then(function (commentDetails) {
          $scope.model.retweets = commentDetails.retweets;
          $scope.model.favorites = commentDetails.favorites;
          $scope.model.replies = commentDetails.replies;
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
    }

    return {
      restrict: 'E',
      replace: true,
      templateUrl: 'directives/comment/commentDirective.html',
      scope: {
        model: '='
      },
      link: CommentLink
    };
  }

  CommentDirective.$inject = ['$window', 'Comment'];

  angular.module('directives').directive('hdComment', CommentDirective);
}(angular));