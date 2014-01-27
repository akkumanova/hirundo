/*global angular*/
(function (angular) {
  'use strict';

  function FavoriteHirundoDirective(Comment) {
    var FavoriteHirundoLink = function ($scope, element, attrs) {
      element.bind('click', function (event) {
        var comment = $scope.$eval(attrs.hdFavoriteHirundo);

        Comment.favorite.save({ commentId: comment.commentId }).$promise
              .then(function (commentDetails) {
          comment.isFavorited = true;
          comment.sharings = commentDetails.sharings;
          comment.favorites = commentDetails.favorites;
        });

        event.stopPropagation();
      });
    };

    return {
      restrict: 'A',
      link: FavoriteHirundoLink
    };
  }

  FavoriteHirundoDirective.$inject = ['Comment'];

  angular.module('directives').directive('hdFavoriteHirundo', FavoriteHirundoDirective);
}(angular));