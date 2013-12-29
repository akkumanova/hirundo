﻿/*global angular*/
(function (angular) {
  'use strict';

  function HomeCtrl($scope, $q, $window, User, Comment) {
    $scope.hirundo = null;
    $scope.loaded = false;

    $scope.sendHirundo = function () {
      var newComment = {
        'Author': $scope.user.userId,
        'Content': $scope.hirundo,
        'PublishDate': new Date()
      };

      return Comment.comment.save(newComment).$promise.then(function () {
        getData();
      });
    };

    var getData = function () {
      $q.all({
        userData: User.userData.get().$promise,
        comments: User.userComments.query({ userId: $window.user.userId }).$promise
      }).then(function (res) {
        $scope.user = res.userData;
        $scope.comments = res.comments;
        $scope.loaded = true;
      });
    };

    getData();
  }

  HomeCtrl.$inject = ['$scope', '$q', '$window', 'User', 'Comment'];

  angular.module('home').controller('home.HomeCtrl', HomeCtrl);
}(angular));
