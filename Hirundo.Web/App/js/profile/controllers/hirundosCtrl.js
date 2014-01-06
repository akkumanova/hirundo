/*global angular*/
(function (angular) {
  'use strict';

  function HirundosCtrl($scope, $stateParams) {
    $scope.userId = $stateParams.id;
  }

  HirundosCtrl.$inject = ['$scope', '$stateParams'];

  angular.module('profile').controller('profile.HirundosCtrl', HirundosCtrl);
}(angular));
