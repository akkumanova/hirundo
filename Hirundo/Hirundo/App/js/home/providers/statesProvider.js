/*global angular*/
(function (angular) {
  'use strict';

  function StatesProvider($stateProvider, navigationStatesProvider) {
    var rootHome = {
      name: 'rootHome',
      url: '/home',
      parent: navigationStatesProvider.states.root,
      'abstract': true,
      views: {
        'pageView': {
          template: '<div ui-view="pageView"></div>'
        }
      }
    };

    this.states = {
      'rootHome': rootHome,
      'home': {
        name: 'home',
        parent: rootHome,
        url: '',
        views: {
          'pageView': {
            templateUrl: 'home/templates/home.html',
            controller: 'home.HomeCtrl'
          }
        }
      }
    };

    $stateProvider
      .state(this.states.rootHome)
      .state(this.states.home);
  }

  StatesProvider.$inject = ['$stateProvider', 'navigation.StatesProvider'];

  StatesProvider.prototype.$get = function () {
    return this.states;
  };

  angular.module('home').provider('home.States', StatesProvider);
}(angular));