/*global angular*/
(function (angular) {
  'use strict';

  function PasswordCtrl($scope, $state, User) {
    $scope.saveClicked = false;
    $scope.passwords = {
      oldPassword: null,
      newPassword: null,
      verifyPassword: null
    };

    $scope.$watch('passwords.oldPassword', function () {
      $scope.passwordInvalid = null;
    });

    $scope.save = function () {
      $scope.saveClicked = true;

      if ($scope.passForm.$valid &&
          $scope.passwords.newPassword === $scope.passwords.verifyPassword) {
        User.userPassword.save($scope.passwords).$promise.then(
          function () {
            $state.go('home');
          },
          function (error) {
            $scope.passwordInvalid = error.data;
          }
        );
      }
    };
  }

  PasswordCtrl.$inject = ['$scope', '$state', 'User'];

  angular.module('settings').controller('settings.PasswordCtrl', PasswordCtrl);
}(angular));
