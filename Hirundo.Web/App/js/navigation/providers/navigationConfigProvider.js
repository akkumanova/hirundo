﻿/*global angular*/
(function (angular) {
  'use strict';

  function NavigationConfigProvider() {
    this.items = [];
  }
  
  NavigationConfigProvider.prototype.$get = function () {
    return {
      items: this.items,
      userFullName: this.userFullName,
      userHasPassword: this.userHasPassword,
      showBreadcrumbBar: this.showBreadcrumbBar
    };
  };
  
  NavigationConfigProvider.prototype.addItem = function (item) {
    this.items.push(item);
    return this;
  };

  angular.module('navigation').provider('navigation.NavigationConfig', NavigationConfigProvider);
}(angular));