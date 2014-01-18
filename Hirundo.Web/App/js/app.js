/*global angular*/
(function (angular) {
  'use strict';
  angular.module('app', [
    'ng',
    'ngResource',
    'ui.router',
    'directives',
    'navigation',
    'home',
    'profile',
    'hirundo',
    'settings'
  ]).config([
    '$urlRouterProvider',
    '$compileProvider',
    '$stateProvider',
    '$locationProvider',
    '$windowProvider',
    'navigation.NavigationConfigProvider',
    function (
      $urlRouterProvider,
      $compileProvider,
      $stateProvider,
      $locationProvider,
      $windowProvider,
      navigationConfigProvider
    ) {
      $locationProvider.html5Mode(false);
      $urlRouterProvider.otherwise('/home');
      $compileProvider.imgSrcSanitizationWhitelist(/^\s*(data|blob):/);

      navigationConfigProvider
        .addItem({ text: 'Home', icon: 'glyphicon-home', state: 'home', items: [] })
        .addItem({
          text: 'Me',
          icon: 'glyphicon-user',
          state: 'profile',
          stateParams: { id: $windowProvider.$get().user.userId },
          items: [
            { text: 'Settings', state: 'settings' },
            { text: 'Sign out', url: '/logout' }
          ]
        });
    }
  ]);
}(angular));
