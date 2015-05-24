/*==============================================================*/
/* Table: T_Manager                                             */
/*==============================================================*/
create table T_Manager (
   ManagerId            bigint               identity,
   UserName             nvarchar(20)         null,
   TrueName             nvarchar(20)         null,
   UserPwd              varchar(200)         null,
   IsUse                bit                  not null default 1,
   constraint PK_T_MANAGER primary key (ManagerId)
)
go

/*==============================================================*/
/* Table: T_MMenu                                               */
/*==============================================================*/
create table T_MMenu (
   MenuId               int                  not null,
   ParentId             int                  not null default 0,
   MenuName             varchar(100)         not null,
   MenuSort             int                  not null,
   IsUse                bit                  not null default 1,
   MenuType             int                  null,
   ActionName           varchar(50)          null,
   ControllerName       varchar(50)          null,
   Params               varchar(256)         null,
   Icon                 varchar(100)         null,
   IsActive             bit                  not null default 0,
   constraint PK_T_MMENU primary key (MenuId)
)
go

/*==============================================================*/
/* Table: T_MOperation                                          */
/*==============================================================*/
create table T_MOperation (
   OperationId          int                  not null,
   OperationName        varchar(100)         null,
   constraint PK_T_MOPERATION primary key (OperationId)
)
go

/*==============================================================*/
/* Table: T_MFunction                                           */
/*==============================================================*/
create table T_MFunction (
   FunctionId           int                  not null,
   FunctionName         varchar(100)         null,
   constraint PK_T_MFUNCTION primary key (FunctionId)
)
go



/*==============================================================*/
/* Table: T_MFunOper                                            */
/*==============================================================*/
create table T_MFunOper (
   OperationId          int                  not null,
   FunctionId           int                  not null,
   constraint PK_T_MFUNOPER primary key (OperationId, FunctionId)
)
go

alter table T_MFunOper
   add constraint FK_T_MFUNOP_REFERENCE_T_MOPERA foreign key (OperationId)
      references T_MOperation (OperationId)
go

alter table T_MFunOper
   add constraint FK_T_MFUNOP_REFERENCE_T_MFUNCT foreign key (FunctionId)
      references T_MFunction (FunctionId)
go


/*==============================================================*/
/* Table: T_MRight                                              */
/*==============================================================*/
create table T_MRight (
   ManagerId            bigint               not null,
   OperationId          int                  not null,
   FunctionId           int                  not null,
   constraint PK_T_MRIGHT primary key (ManagerId, OperationId, FunctionId)
)
go

alter table T_MRight
   add constraint FK_T_MRIGHT_REFERENCE_T_MANAGE foreign key (ManagerId)
      references T_Manager (ManagerId)
go

alter table T_MRight
   add constraint FK_T_MRIGHT_REFERENCE_T_MFUNOP foreign key (OperationId, FunctionId)
      references T_MFunOper (OperationId, FunctionId)
go




/*==============================================================*/
/* Table: T_MMenuFunOper                                        */
/*==============================================================*/


create table T_MMenuFunOper (
   MenuId               int                  not null,
   OperationId          int                  not null,
   FunctionId           int                  not null,
   constraint PK_T_MMENUFUNOPER primary key (MenuId, OperationId, FunctionId)
)
go

alter table T_MMenuFunOper
   add constraint FK_T_MMENUF_REFERENCE_T_MMENU foreign key (MenuId)
      references T_MMenu (MenuId)
go

alter table T_MMenuFunOper
   add constraint FK_T_MMENUF_REFERENCE_T_MFUNOP foreign key (OperationId, FunctionId)
      references T_MFunOper (OperationId, FunctionId)
go



/*==============================================================*/
/* Table: T_MGroup                                              */
/*==============================================================*/
create table T_MGroup (
   GroupId              int                  identity,
   GroupName            varchar(100)         not null,
   GroupMemo            varchar(200)         null,
   constraint PK_T_MGROUP primary key (GroupId)
)
go



/*==============================================================*/
/* Table: T_MManagerGroup                                       */
/*==============================================================*/
create table T_MManagerGroup (
   ManagerId            bigint               not null,
   GroupId              int                  not null,
   constraint PK_T_MMANAGERGROUP primary key (ManagerId, GroupId)
)
go

alter table T_MManagerGroup
   add constraint FK_T_MMANAG_REFERENCE_T_MANAGE foreign key (ManagerId)
      references T_Manager (ManagerId)
go

alter table T_MManagerGroup
   add constraint FK_T_MMANAG_REFERENCE_T_MGROUP foreign key (GroupId)
      references T_MGroup (GroupId)
go


/*==============================================================*/
/* Table: T_MGroupRight                                         */
/*==============================================================*/
create table T_MGroupRight (
   GroupId              int                  not null,
   OperationId          int                  not null,
   FunctionId           int                  not null,
   constraint PK_T_MGROUPRIGHT primary key (GroupId, OperationId, FunctionId)
)
go

create view V_MFunOper
as
SELECT   a.OperationId, a.FunctionId, b.FunctionName, c.OperationName
FROM      dbo.T_MFunOper AS a INNER JOIN
                dbo.T_MFunction AS b ON a.FunctionId = b.FunctionId INNER JOIN
                dbo.T_MOperation AS c ON a.OperationId = c.OperationId

go

create view V_MManagerGroup as
 select b.OperationId, b.FunctionId, b.GroupId, a.ManagerId from T_MManagerGroup a join T_MGroupRight b on a.GroupId = b.GroupId
go




/*==============================================================*/
/* Table: T_Template                                            */
/*==============================================================*/
create table T_Template (
   TemplateId           int                  not null default 1,
   TemplateName         varchar(128)         not null,
   TemplateContent      varchar(Max)         null,
   constraint PK_T_TEMPLATE primary key (TemplateId)
)
go




alter table T_MGroupRight
   add constraint FK_T_MGROUP_REFERENCE_T_MGROUP foreign key (GroupId)
      references T_MGroup (GroupId)
go

alter table T_MGroupRight
   add constraint FK_T_MGROUP_REFERENCE_T_MFUNOP foreign key (OperationId, FunctionId)
      references T_MFunOper (OperationId, FunctionId)
go



/*==============================================================*/
/* Table: T_Image                                               */
/*==============================================================*/
create table T_Image (
   ImageId              bigint               identity,
   ImageName            varchar(128)         not null,
   ImageExt             varchar(10)          null,
   ImageSize            int                  null,
   ImageWidth           int                  null,
   ImageHeight          int                  null,
   UserId               bigint               not null,
   constraint PK_T_IMAGE primary key (ImageId)
)
go



SET IDENTITY_INSERT [T_Manager] ON
Insert Into [T_Manager] ([ManagerId],[UserName],[UserPwd],[Truename]) Values('1','admin','bZgMBsx1WYkUQ13jaqIyFw==','管理员')
SET IDENTITY_INSERT [T_Manager] OFF

Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('10','操作员')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('11','操作员组')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('20','日志')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('30','平台')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('40','平台功能')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('50','平台操作')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('55','平台功能操作')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('60','用户')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('65','微信')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('70','产品')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('71','产品分类')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('80','订单')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('90010','菜单功能操作')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('90020','功能操作')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('90030','功能')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('90040','操作')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('90050','菜单')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('90060','模板')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('90070','公告')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('90080','消息')
Insert Into [T_MFunction] ([FunctionId],[FunctionName]) Values('90090','样式')

Insert Into [T_MOperation] ([OperationId],[OperationName]) Values('10','查看')
Insert Into [T_MOperation] ([OperationId],[OperationName]) Values('20','添加')
Insert Into [T_MOperation] ([OperationId],[OperationName]) Values('30','修改')
Insert Into [T_MOperation] ([OperationId],[OperationName]) Values('40','删除')
Insert Into [T_MOperation] ([OperationId],[OperationName]) Values('50','赋权')
Insert Into [T_MOperation] ([OperationId],[OperationName]) Values('60','修改密码')
Insert Into [T_MOperation] ([OperationId],[OperationName]) Values('70','分组')
Insert Into [T_MOperation] ([OperationId],[OperationName]) Values('80','产品价格')
Insert Into [T_MOperation] ([OperationId],[OperationName]) Values('90','产品分类')
Insert Into [T_MOperation] ([OperationId],[OperationName]) Values('100','产品功能操作')
Insert Into [T_MOperation] ([OperationId],[OperationName]) Values('110','发消息')

Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('10','10')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('11','10')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('30','10')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('40','10')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('50','10')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('55','10')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('70','10')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('90010','10')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('90020','10')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('10','20')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('11','20')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('30','20')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('40','20')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('50','20')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('70','20')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('10','30')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('11','30')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('30','30')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('40','30')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('50','30')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('55','30')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('70','30')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('90010','30')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('90020','30')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('10','40')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('11','40')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('30','40')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('40','40')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('50','40')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('70','40')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('10','50')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('11','50')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('10','60')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('10','70')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('70','80')
Insert Into [T_MFunOper] ([FunctionId],[OperationId]) Values('70','90')


Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('200','0','产品管理','200',1,'1','','','','',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('210','200','分类','20',1,'2','category','product','','icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('220','200','产品','30',1,'2','product','product','','icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('300','0','用户管理','300',1,'1','','','','',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('400','0','订单管理','400',1,'1','','','','',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('1000','0','系统管理','1000',1,'1',null,null,null,'',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('1010','1000','操作员','1010',1,'2','manager','sys',null,'icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('1020','1000','操作员组','1020',1,'2','group','sys',null,'icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9000','1000','功能','9000',1,'2','function','sys',null,'icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9010','1000','操作','9010',1,'2','operation','sys',null,'icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9020','1000','功能操作','9020',1,'2','funoper','sys',null,'icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9030','1000','菜单','9030',1,'2','menu','sys',null,'icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9040','1000','菜单功能操作','9040',1,'2','menufunoper','sys',null,'icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9100','1000','日志','9100',1,'2','log','log','','icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9120','300','用户','20',1,'2','users','user','','icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9140','400','订单','20',1,'2','orders','order','','icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9160','300','微信配置','40',1,'2','weixin_s','weixin','','icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9180','300','消息','60',1,'2','messages','message',null,'icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9220','1000','模板','9120',1,'2','template','template',null,'icon-star-empty',0)
Insert Into [T_MMenu] ([MenuId],[ParentId],[MenuName],[MenuSort],[IsUse],[MenuType],[ActionName],[ControllerName],[Params],[Icon],[IsActive]) Values('9240','200','平台','10',1,'2','plat','plat','','icon-star-empty',0)


Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1010','10','10')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1010','20','10')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1010','30','10')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1010','40','10')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1010','50','10')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1010','60','10')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1010','70','10')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1020','10','11')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1020','20','11')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1020','30','11')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1020','40','11')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('1020','50','11')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('9020','10','90020')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('9020','30','90020')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('9040','10','90010')
Insert Into [T_MMenuFunOper] ([MenuId],[OperationId],[FunctionId]) Values('9040','30','90010')



Insert Into [T_Template] ([TemplateId],[TemplateName],[TemplateContent]) Values('1','找回密码模板','<b>尊敬的用户： <br />
<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;{{ username }},您好！ <br />
</b> 
<p>
	<b> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;您在{{ datetime }}提交找回密码请求，请点击下面的链接修改用户{{ username }}的密码: </b> 
</p>
<b> <a href="{{ url }}" target="_blank">{{ url }}</a> <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(如果您无法点击这个链接，请将此链接复制到浏览器地址栏后访问) <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;为了保证您帐号的安全性，该链接有效期为{{ hours }}小时，并且点击一次后将失效! <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;设置并牢记密码保护问题将更好地保障您的帐号安全。 <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;如果您误收到此电子邮件，则可能是其他用户在尝试帐号设置时的误操作，如果您并未发起该请求，则无需再进行任何操作，并可以放心地忽略此电子邮件。 <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;若您担心帐号安全，建议您立即登录修改密码。 <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br />
<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; {{ datetime }}<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>{{ title }}</b><br />
此邮件为自动发送，请勿回复！ </b>')