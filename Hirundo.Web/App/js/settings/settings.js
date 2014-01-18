/*global angular*/
(function (angular) {
  'use strict';
  angular.module('settings', [
    'ng',
    'navigation',
    'imageupload',
    'settings.templates'
  ]).config([
    '$urlRouterProvider',
    '$stateProvider',
    function ($urlRouterProvider, $stateProvider) {
      $urlRouterProvider.when('/settings', '/settings/profile');

      $stateProvider
        .state({
          name: 'settings',
          parent: 'root',
          url: '/settings',
          views: {
            'pageView@root': {
              templateUrl: 'settings/templates/settings.html',
              controller: 'settings.SettingsCtrl'
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
