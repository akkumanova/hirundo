/*global angular*/
(function (angular) {
  'use strict';

  function DeleteHirundoDirective($modal, $state, Comment) {
    var DeleteHirundoLink = function ($scope, element, attrs) {
      var DeleteModalCtrl = function ($scope, $modalInstance, commentData) {
        $scope.commentData = commentData;

        $scope.cancel = function () {
          $modalInstance.close(false);
        };

        $scope.ok = function () {
          $modalInstance.close(true);
        };
      };

      element.bind('click', function (event) {
        var comment = $scope.$eval(attrs.hdDeleteHirundo);

        var modalInstance = $modal.open({
          templateUrl: 'directives/deleteHirundo/deleteModal.html',
          controller: DeleteModalCtrl,
          windowClass: 'comment-modal',
          resolve: {
            commentData: function () {
              return {
                author: comment.author,
                authorImg: comment.authorImg,
                publishDate: comment.publishDate,
                content: comment.content,
                image: comment.image,
                location: comment.location
              };
            }
          }
        });

        modalInstance.result.then(function (result) {
          if (result) {
            var commentId = $scope.$eval(attrs.hdDeleteHirundo).commentId;

            Comment.comment.remove({ commentId: commentId }).$promise.then(function () {
              $state.go('home', {}, { reload: true });
            });
          }
        });

        event.stopPropagation();
      });
    };

    return {
      restrict: 'A',
      link: DeleteHirundoLink
    };
  }

  DeleteHirundoDirective.$inject = [
    '$modal',
    '$state',
    'Comment'
  ];

  angular.module('directives').directive('hdDeleteHirundo', DeleteHirundoDirective);
}(angular));