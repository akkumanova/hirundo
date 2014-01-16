/*global angular*/
(function (angular) {
  'use strict';
  angular.module('navigation', [
    'ng',
    'ui.router',
    'ui.bootstrap',
    'navigation.templates'
  ]).config(['$stateProvider', function ($stateProvider) {
    $stateProvider.state({
      name: 'root',
      views: {
        'rootView': {
          templateUrl: 'navigation/templates/navbar.html'
        }
      }
    })
    .state({
      name: 'search',
      parent: 'root',
      url: '/search?username',
      views: {
        'pageView@root': {
          templateUrl: 'navigation/templates/searchResults.html',
          controller: 'navigation.SearchResultsCtrl'
        }
      }
    });
  }]);
}(angular));
