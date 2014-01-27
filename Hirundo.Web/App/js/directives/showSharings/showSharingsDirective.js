/*global angular*/
(function (angular) {
  'use strict';

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

  function ShowSharingsDirective($modal, $window, Comment) {
    var ShowSharingsLink = function ($scope, element, attrs) {
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
                image: comment.image,
                location: comment.location,
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
      link: ShowSharingsLink
    };
  }

  ShowSharingsDirective.$inject = ['$modal', '$window', 'Comment'];

  angular.module('directives').directive('hdShowSharings', ShowSharingsDirective);
}(angular));