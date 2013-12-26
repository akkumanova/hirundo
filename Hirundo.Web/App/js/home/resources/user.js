/*global angular*/
(function (angular) {
  'use strict';

  angular.module('home').factory('User', ['$resource', function ($resource) {
    return $resource('api/user');
  }]);
}(angular));
