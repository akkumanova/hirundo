/*global angular*/
( function ( angular ) {
  'use strict';

  function FavoritesCtrl( $scope, $stateParams ) {
    $scope.userId = $stateParams.id;
  }

  FavoritesCtrl.$inject = ['$scope', '$stateParams'];

  angular.module( 'profile' ).controller( 'profile.FavoritesCtrl', FavoritesCtrl );
}( angular ) );
