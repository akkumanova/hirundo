/*global angular*/
(function (angular) {
  'use strict';

  function HomeCtrl($scope, User, Comment) {
    User.get().$promise.then(function (user) {
      $scope.user = user;

      Comment.get({ userId: user.userId }).$promise.then(function (commentsObj) {
        $scope.user.commentsCount = commentsObj.count;
      });
    });

    $scope.hirundo = null;

    $scope.sendHirundo = function () {
      var newComment = {
        'Author': $scope.user.userId,
        'Content': $scope.hirundo,
        'PublishDate': new Date()
      };

      return Comment.save(newComment).$promise;
    };
  }

  HomeCtrl.$inject = ['$scope', 'User', 'Comment'];

  angular.module('home').controller('home.HomeCtrl', HomeCtrl);
}(angular));
