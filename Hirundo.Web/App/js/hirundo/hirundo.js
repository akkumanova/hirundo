/*global angular*/
(function (angular) {
  'use strict';
  angular.module('hirundo', [
    'ng',
    'ui.router',
    'ui.bootstrap',
    'angularMoment',
    'navigation',
    'hirundo.templates'
  ]).config(['$stateProvider', function ($stateProvider) {
    $stateProvider
      .state({
        name: 'hirundo',
        parent: 'root',
        url: '/hirundo/:id',
        views: {
          'pageView@root': {
            templateUrl: 'hirundo/templates/hirundo.html',
            controller: 'hirundo.HirundoCtrl'
          }
        }
      });
  }]);
}(angular));
