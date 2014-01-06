/*global angular*/
(function (angular) {
  'use strict';

  angular.module('app').factory('User', ['$resource', function ($resource) {
    var userFactory = {
      userData: $resource('api/user/:userId', { userId: '@userId' }),
      userTimeline: $resource('api/user/:userId/timeline', { userId: '@userId' }),
      userComments: $resource('api/user/:userId/comments', { userId: '@userId' })
    };

    return userFactory;
  }]);
}(angular));
