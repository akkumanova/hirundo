// Usage: <hd-comments model="<model_name>"></hd-comments>

/*global angular, $*/
(function (angular, $) {
  'use strict';

  function CommentDirective($window, Comment) {
    function CommentLink($scope, element) {
      var commentImg = $(element).find('img.comment-image');
      commentImg.load(function () {
        resizeImage();
      });

      $scope.userId = $window.user.userId;
      $scope.userImg = $window.user.userImg;

      $scope.sendReply = function (hirundo) {
        var commentId = $scope.model.commentId;
        var reply = {
          'Author': $window.user.userId,
          'Content': hirundo.content,
          'Location': hirundo.location,
          'Image': hirundo.image,
          'PublishDate': new Date()
        };

        return Comment.reply.save({ commentId: commentId }, reply).$promise
                            .then(function (commentDetails) {
          $scope.model.sharings = commentDetails.sharings;
          $scope.model.favorites = commentDetails.favorites;
          $scope.model.replies = commentDetails.replies;
        });
      };

      $scope.commentClick = function () {
        $scope.model.isExpanded = !$scope.model.isExpanded;

        if ($scope.model.isExpanded) {
          Comment.commentDetails.get({ commentId: $scope.model.commentId })
                 .$promise.then(function (commentDetails) {
            $scope.model.sharings = commentDetails.sharings;
            $scope.model.favorites = commentDetails.favorites;
            $scope.model.replies = commentDetails.replies;
          });
        }

        resizeImage();
      };

      function resizeImage() {
        var margin = (250 - commentImg.height()) / 2;

        if (commentImg.height() > 250 && !$scope.model.isExpanded) {
          commentImg.css('margin-top', margin + 'px');
          commentImg.css('margin-bottom', margin + 'px');
        }
        else {
          commentImg.css('margin-top', 'auto');
          commentImg.css('margin-bottom', 'auto');
        }
      }
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
}(angular, $));