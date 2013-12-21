/*global angular*/
(function (angular) {
  'use strict';
  angular.module('app', [
    'ng',
    'ui.router',
    'navigation',
    'home'
  ]).config([
    '$urlRouterProvider',
    '$stateProvider',
    '$locationProvider',
    'navigation.NavigationConfigProvider',
    function (
      $urlRouterProvider,
      $stateProvider,
      $locationProvider,
      navigationConfigProvider
    ) {
      $locationProvider.html5Mode(false);
      $urlRouterProvider.otherwise('/home');

      navigationConfigProvider
        .addItem({ text: 'Home', icon: 'glyphicon-home', url: '/home', items: [] })
        .addItem({ text: 'Connect', url: '/connect', items: [] })
        .addItem({ text: 'Discover', url: '/discover', items: [] })
        .addItem({
          text: 'Me',
          icon: 'glyphicon-user',
          url: '/me',
          items: [
            { text: 'Edit profile', url: '/edit' },
            { text: 'Settings', url: '/settings' },
            { text: 'Sign out', url: '/signOut' }
          ]
        });
    }
  ]);
}(angular));
