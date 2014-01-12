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
    'hirundo'
  ]).config([
    '$urlRouterProvider',
    '$stateProvider',
    '$locationProvider',
    '$windowProvider',
    'navigation.NavigationConfigProvider',
    function (
      $urlRouterProvider,
      $stateProvider,
      $locationProvider,
      $windowProvider,
      navigationConfigProvider
    ) {
      $locationProvider.html5Mode(false);
      $urlRouterProvider.otherwise('/home');

      navigationConfigProvider
        .addItem({ text: 'Home', icon: 'glyphicon-home', state: 'home', items: [] })
        .addItem({
          text: 'Me',
          icon: 'glyphicon-user',
          state: 'profile',
          stateParams: { id: $windowProvider.$get().user.userId },
          items: [
            { text: 'Settings', url: '/settings' },
            { text: 'Sign out', url: '/logout' }
          ]
        });
    }
  ]);
}(angular));
