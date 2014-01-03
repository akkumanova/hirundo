/*global angular*/
(function (angular) {
  'use strict';

  angular.module('home').factory('User', ['$resource', function ($resource) {
    var userFactory = {
      userData: $resource('api/user/:userId', { userId: '@userId' }),
      userComments: $resource('api/user/:userId/comments', { userId: '@userId' })
    };

    return userFactory;
  }]);
}(angular));
