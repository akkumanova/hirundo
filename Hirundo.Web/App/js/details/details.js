/*global angular*/
(function (angular) {
  'use strict';
  angular.module('details', [
    'ng',
    'navigation',
    'details.templates'
  ]).config(['$stateProvider', function ($stateProvider) {
    $stateProvider
      .state({
        name: 'rootDetails',
        url: '/details',
        parent: 'root',
        'abstract': true
      })
      .state({
        name: 'details',
        parent: 'rootDetails',
        url: '',
        views: {
          'pageView@root': {
            templateUrl: 'details/templates/details.html',
            controller: 'details.DetailsCtrl'
          }
        }
      });
  }]);
}(angular));
