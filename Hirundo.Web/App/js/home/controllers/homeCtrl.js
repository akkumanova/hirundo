/*global angular*/
(function (angular) {
  'use strict';

  function HomeCtrl($scope, $window, $state, User, Comment) {
    $scope.loaded = false;

    User.userData.get({ userId: $window.user.userId }).$promise.then(function (user) {
      $scope.user = user;
      $scope.loaded = true;

      $window.user.userImg = $scope.user.image;
    });

    $scope.sendHirundo = function (hirundo) {
      var newComment = {
        Author: $scope.user.userId,
        Content: hirundo.content,
        Location: hirundo.location,
        Image: hirundo.image,
        PublishDate: new Date()
      };

      return Comment.comment.save(newComment).$promise.then(function () {
        $state.go('home', {}, { reload: true });
      });
    };
  }

  HomeCtrl.$inject = ['$scope', '$window', '$state', 'User', 'Comment'];

  angular.module('home').controller('home.HomeCtrl', HomeCtrl);
}(angular));
