/*global angular*/
(function (angular) {
  'use strict';

  function HomeCtrl($scope, $window, $state, User, Comment) {
    $scope.hirundo = null;
    $scope.loaded = false;

    User.userData.get({ userId: $window.user.userId }).$promise.then(function (user) {
      $scope.user = user;
      $scope.loaded = true;

      $window.user.userImage = $scope.user.image;
    });

    $scope.sendHirundo = function () {
      var newComment = {
        'Author': $scope.user.userId,
        'Content': $scope.hirundo,
        'PublishDate': new Date()
      };

      return Comment.comment.save(newComment).$promise.then(function () {
        $state.go('home', {}, { reload: true });
      });
    };
  }

  HomeCtrl.$inject = ['$scope', '$window', '$state', 'User', 'Comment'];

  angular.module('home').controller('home.HomeCtrl', HomeCtrl);
}(angular));
