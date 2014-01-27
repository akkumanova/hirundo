/*global angular*/
(function (angular) {
  'use strict';
  angular.module('profile', [
    'ng',
    'navigation',
    'profile.templates'
  ]).config(['$stateProvider', function ($stateProvider) {
    $stateProvider
      .state({
        name: 'profile',
        parent: 'root',
        url: '/profile/:id',
        views: {
          'pageView@root': {
            templateUrl: 'profile/templates/profile.html',
            controller: 'profile.ProfileCtrl'
          }
        }
      })
      .state({
        name: 'profile.hirundos',
        parent: 'profile',
        url: '/hirundos',
        views: {
          'personView@profile': {
            templateUrl: 'profile/templates/hirundos.html',
            controller: 'profile.HirundosCtrl'
          }
        }
      })
      .state({
        name: 'profile.favorites',
        parent: 'profile',
        url: '/favorites',
        views: {
          'personView@profile': {
            templateUrl: 'profile/templates/favorites.html',
            controller: 'profile.FavoritesCtrl'
          }
        }
      })
      .state({
        name: 'profile.followers',
        parent: 'profile',
        url: '/followers',
        views: {
          'personView@profile': {
            templateUrl: 'profile/templates/followers.html',
            controller: 'profile.FollowersCtrl'
          }
        }
      })
      .state({
        name: 'profile.following',
        parent: 'profile',
        url: '/following',
        views: {
          'personView@profile': {
            templateUrl: 'profile/templates/following.html',
            controller: 'profile.FollowingCtrl'
          }
        }
      });
  }]);
}(angular));
