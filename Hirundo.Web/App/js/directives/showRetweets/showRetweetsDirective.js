/*global angular*/
(function (angular) {
  'use strict';

  function ShowRetweetsDirective($modal, $window, Comment) {
    var ShowRetweetsLink = function ($scope, element, attrs) {
      var RetweetModalCtrl = function ($scope, $modalInstance, commentData, users) {
        $scope.commentData = commentData;
        $scope.header = 'Retweeted ' +
                        commentData.retweets +
                        (commentData.retweets === 1 ? ' time.' : ' times.');
        $scope.loading = true;

        users.$promise.then(function (result) {
          $scope.users = result;
          $scope.loading = false;
        });

        $scope.close = function () {
          $modalInstance.dismiss('cancel');
        };
      };

      element.bind('click', function (event) {
        var comment = $scope.$eval(attrs.hdShowRetweets);

        $modal.open({
          templateUrl: 'directives/showRetweets/retweetsModal.html',
          controller: RetweetModalCtrl,
          windowClass: 'users-modal',
          resolve: {
            commentData: function () {
              return {
                author: comment.author,
                authorImg: comment.authorImg,
                publishDate: comment.publishDate,
                content: comment.content,
                retweets: comment.retweets
              };
            },
            users: function () {
              return Comment.retweet.query({ commentId: comment.commentId });
            }
          }
        });

        event.stopPropagation();
      });
    };

    return {
      restrict: 'A',
      compile: function compile(tElement) {
        tElement.attr('style', 'cursor: pointer;');

        return ShowRetweetsLink;
      }
    };
  }

  ShowRetweetsDirective.$inject = ['$modal', '$window', 'Comment'];

  angular.module('directives').directive('hdShowRetweets', ShowRetweetsDirective);
}(angular));