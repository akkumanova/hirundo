/*global angular*/
(function (angular) {
  'use strict';

  angular.module('home').factory('Comment', ['$resource', function ($resource) {
    var commentFactory = {
      comment: $resource('api/comments/:commentId', { commentId: '@commentId' }),
      commentDetails: $resource('api/comments/:commentId/details', { commentId: '@commentId' }),
      reply: $resource('api/comments/:commentId/reply', { commentId: '@commentId' })
    };

    return commentFactory;
  }]);
}(angular));
