/*global angular*/
(function (angular) {
  'use strict';

  angular.module('app').factory('User', ['$resource', function ($resource) {
    var userFactory = {
      userData: $resource('api/user/:userId', { userId: '@userId' }),
      userProfile: $resource('api/user/profile'),
      userTimeline: $resource('api/user/:userId/timeline', { userId: '@userId' }),
      userComments: $resource( 'api/user/:userId/comments', { userId: '@userId' } ),
      userFavorites: $resource('api/user/:userId/favorites', { userId: '@userId' }),
      userFollowers: $resource('api/user/:userId/followers', { userId: '@userId' }),
      userFollowing: $resource('api/user/:userId/following', { userId: '@userId' })
    };

    return userFactory;
  }]);
}(angular));
