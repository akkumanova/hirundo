/*global angular*/
(function (angular) {
  'use strict';

  angular.module('home').factory('Comment', ['$resource', function ($resource) {
    return $resource('api/comments/:commentId', { userId: '@commentId' });
  }]);
}(angular));
