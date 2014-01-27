/*global angular*/
(function (angular) {
  'use strict';

  var FavoritesModalCtrl = function ($scope, $modalInstance, commentData, users) {
    $scope.commentData = commentData;
    $scope.header = 'Favorited ' +
                    commentData.favorites +
                    (commentData.favorites === 1 ? ' time.' : ' times.');
    $scope.loading = true;

    users.$promise.then(function (result) {
      $scope.users = result;
      $scope.loading = false;
    });

    $scope.close = function () {
      $modalInstance.dismiss('cancel');
    };
  };

  function ShowFavoritesDirective($modal, $window, Comment) {
    var ShowFavoritesLink = function ($scope, element, attrs) {
      element.bind('click', function (event) {
        var comment = $scope.$eval(attrs.hdShowFavorites);

        $modal.open({
          templateUrl: 'directives/showFavorites/favoritesModal.html',
          controller: FavoritesModalCtrl,
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
                favorites: comment.favorites
              };
            },
            users: function () {
              return Comment.favorite.query({ commentId: comment.commentId });
            }
          }
        });

        event.stopPropagation();
      });
    };

    return {
      restrict: 'A',
      link: ShowFavoritesLink
    };
  }

  ShowFavoritesDirective.$inject = ['$modal', '$window', 'Comment'];

  angular.module('directives').directive('hdShowFavorites', ShowFavoritesDirective);
}(angular));