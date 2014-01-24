/*global angular*/
(function (angular) {
  'use strict';

  function ShowSharingsDirective($modal, $window, Comment) {
    var ShowSharingsLink = function ($scope, element, attrs) {
      var SharingsModalCtrl = function ($scope, $modalInstance, commentData, users) {
        $scope.commentData = commentData;
        $scope.header = 'Shared ' +
                        commentData.sharings +
                        (commentData.sharings === 1 ? ' time.' : ' times.');
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
        var comment = $scope.$eval(attrs.hdShowSharings);

        $modal.open({
          templateUrl: 'directives/showSharings/sharingsModal.html',
          controller: SharingsModalCtrl,
          windowClass: 'users-modal',
          resolve: {
            commentData: function () {
              return {
                author: comment.author,
                authorImg: comment.authorImg,
                publishDate: comment.publishDate,
                content: comment.content,
                sharings: comment.sharings
              };
            },
            users: function () {
              return Comment.share.query({ commentId: comment.commentId });
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

        return ShowSharingsLink;
      }
    };
  }

  ShowSharingsDirective.$inject = ['$modal', '$window', 'Comment'];

  angular.module('directives').directive('hdShowSharings', ShowSharingsDirective);
}(angular));