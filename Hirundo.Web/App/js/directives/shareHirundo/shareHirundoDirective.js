/*global angular*/
(function (angular) {
  'use strict';

  function ShareHirundoDirective($modal, Comment) {
    var ShareHirundoLink = function ($scope, element, attrs) {
      var ShareModalCtrl = function ($scope, $modalInstance, commentData) {
        $scope.commentData = commentData;

        $scope.cancel = function () {
          $modalInstance.close(false);
        };

        $scope.ok = function () {
          $modalInstance.close(true);
        };
      };

      element.bind('click', function (event) {
        var comment = $scope.$eval(attrs.hdShareHirundo);

        var modalInstance = $modal.open({
          templateUrl: 'directives/shareHirundo/shareModal.html',
          controller: ShareModalCtrl,
          windowClass: 'comment-modal',
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
            Comment.share.save({ commentId: $scope.$eval(attrs.hdShareHirundo).commentId })
                  .$promise.then(function (commentDetails) {
              comment.isShared = true;
              comment.sharings = commentDetails.sharings;
              comment.favorites = commentDetails.favorites;
            });
          }
        });

        event.stopPropagation();
      });
    };

    return {
      restrict: 'A',
      compile: function compile(tElement) {
        tElement.attr('style', 'cursor: pointer;');

        return ShareHirundoLink;
      }
    };
  }

  ShareHirundoDirective.$inject = [
    '$modal',
    'Comment'
  ];

  angular.module('directives').directive('hdShareHirundo', ShareHirundoDirective);
}(angular));