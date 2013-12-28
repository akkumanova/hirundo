/*global angular*/
(function (angular) {
  'use strict';
  angular.module('home', [
    'ng',
    'ui.router',
    'ui.bootstrap',
    'angularMoment',
    'navigation',
    'home.templates'
  ]).config(['$stateProvider', function ($stateProvider) {
    $stateProvider
      .state({
        name: 'rootHome',
        url: '/home',
        parent: 'root',
        'abstract': true
      })
      .state({
        name: 'home',
        parent: 'rootHome',
        url: '',
        views: {
          'pageView@root': {
            templateUrl: 'home/templates/home.html',
            controller: 'home.HomeCtrl'
          }
        }
      });
  }]);
}(angular));
