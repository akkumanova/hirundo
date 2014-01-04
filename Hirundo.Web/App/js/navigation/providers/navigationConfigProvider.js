/*global angular*/
(function (angular) {
  'use strict';

  function NavigationConfigProvider() {
    this.items = [];
    this.loading = false;
  }
  
  NavigationConfigProvider.prototype.$get = function () {
    return {
      items: this.items,
      loading: this.loading
    };
  };
  
  NavigationConfigProvider.prototype.addItem = function (item) {
    this.items.push(item);
    return this;
  };

  angular.module('navigation').provider('navigation.NavigationConfig', NavigationConfigProvider);
}(angular));