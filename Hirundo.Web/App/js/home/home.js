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
        name: 'home',
        parent: 'root',
        url: '/home',
        views: {
          'pageView@root': {
            templateUrl: 'home/templates/home.html',
            controller: 'home.HomeCtrl'
          }
        }
      });
  }]);
}(angular));
