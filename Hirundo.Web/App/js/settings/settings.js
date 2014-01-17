/*global angular*/
(function (angular) {
  'use strict';
  angular.module('settings', [
    'ng',
    'navigation',
    'settings.templates'
  ]).config([
    '$urlRouterProvider',
    '$stateProvider',
    function ($urlRouterProvider, $stateProvider) {
      $urlRouterProvider.when('/settings', '/settings/account');

      $stateProvider
        .state({
          name: 'settings',
          parent: 'root',
          url: '/settings',
          views: {
            'pageView@root': {
              templateUrl: 'settings/templates/settings.html'
            }
          }
        })
        .state({
          name: 'settings.account',
          parent: 'settings',
          url: '/account',
          views: {
            'settingsView@settings': {
              templateUrl: 'settings/templates/account.html',
              controller: 'settings.AccountCtrl'
            }
          }
        })
        .state({
          name: 'settings.password',
          parent: 'settings',
          url: '/password',
          views: {
            'settingsView@settings': {
              templateUrl: 'settings/templates/password.html',
              controller: 'settings.PasswordCtrl'
            }
          }
        })
        .state({
          name: 'settings.profile',
          parent: 'settings',
          url: '/profile',
          views: {
            'settingsView@settings': {
              templateUrl: 'settings/templates/profile.html',
              controller: 'settings.ProfileCtrl'
            }
          }
        });
    }
  ]);
}(angular));
