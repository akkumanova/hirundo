/*global angular*/
(function (angular) {
  'use strict';

  angular.module('app').factory('Comment', ['$resource', function ($resource) {
    var commentFactory = {
      comment: $resource('api/comments/:commentId', { commentId: '@commentId' }),
      commentDetails: $resource('api/comments/:commentId/details', { commentId: '@commentId' }),
      reply: $resource('api/comments/:commentId/reply', { commentId: '@commentId' }),
      share: $resource('api/comments/:commentId/share', { commentId: '@commentId' }),
      favorite: $resource('api/comments/:commentId/favorite', { commentId: '@commentId' })
    };

    return commentFactory;
  }]);
}(angular));
