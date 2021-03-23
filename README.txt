

UserName: admin
Password: 123456



The purpose of this application is to use the actions of the controllers as role and generate menu based on roles. I've shown it here, how to do this. For that purpose I've created a custom authorize attribute named CustomAuthorizeAttribute that takes controller name and the action name as parametera and applied it on the action as attribute where I want to apply. I've created a RolesForMenu static class to create role based menu. It has two static methods: GetMenus and GetMenu.I've called GetMenus from Layout to generate role based menu. Then I've called GetMenu from controller's Index action to pass only authorized action link to the Index view.