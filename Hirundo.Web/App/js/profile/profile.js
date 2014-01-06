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
        name: 'rootProfile',
        url: '/profile',
        parent: 'root',
        'abstract': true
      })
      .state({
        name: 'profile',
        parent: 'rootProfile',
        url: '/:id',
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
      });
  }]);
}(angular));
