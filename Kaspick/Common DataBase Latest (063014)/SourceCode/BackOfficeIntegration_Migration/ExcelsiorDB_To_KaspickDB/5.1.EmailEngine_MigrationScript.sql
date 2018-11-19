SET NOCOUNT ON

-- TBL_EML_EmailTemplate_Type script
SET IDENTITY_INSERT dbo.TBL_EML_EmailTemplate_Type ON

INSERT INTO TBL_EML_EmailTemplate_Type (
	email_template_type_id
	,email_template_type_name
	)
SELECT email_template_type_id
	,email_template_type_name
FROM $(ExcelsiorDB)..EML_EmailTemplate_Type

SET IDENTITY_INSERT dbo.TBL_EML_EmailTemplate_Type OFF
-- TBL_EML_EmailTemplate script
SET IDENTITY_INSERT dbo.TBL_EML_EmailTemplate ON

INSERT INTO TBL_EML_EmailTemplate (
	email_template_id
	,email_template_name
	,subject
	,body
	,contains_html
	,email_template_type_id
	,priority_flag
	)
SELECT email_template_id
	,email_template_name
	,subject
	,body
	,contains_html
	,email_template_type_id
	,priority_flag
FROM $(ExcelsiorDB)..EML_EmailTemplate

SET IDENTITY_INSERT dbo.TBL_EML_EmailTemplate OFF

-- TBL_EML_EmailQueue script
SET IDENTITY_INSERT dbo.TBL_EML_EmailQueue ON

INSERT INTO TBL_EML_EmailQueue (
	email_id
	,from_email_address
	,reply_to_email_address
	,bounce_back_email_address
	,to_email_address
	,cc_email_address
	,bcc_email_address
	,subject
	,body
	,is_html_email
	,is_sent
	,sent_datetime
	,created_datetime
	,created_user_id
	,created_proxy_webuser_id
	,created_webuser_id
	,email_template_id
	,priority_flag
	)
SELECT email_id
	,from_email_address
	,reply_to_email_address
	,bounce_back_email_address
	,to_email_address
	,cc_email_address
	,bcc_email_address
	,subject
	,body
	,is_html_email
	,is_sent
	,sent_datetime
	,created_datetime
	,created_user_id
	,created_proxy_webuser_id
	,created_webuser_id
	,email_template_id
	,priority_flag
FROM $(ExcelsiorDB)..EML_EmailQueue

SET IDENTITY_INSERT dbo.TBL_EML_EmailQueue OFF
-- TBL_EML_EmailTemplateRule script
SET IDENTITY_INSERT dbo.TBL_EML_EmailTemplateRule ON

INSERT INTO TBL_EML_EmailTemplateRule (
	email_template_rule_id
	,email_template_rule_name
	,email_template_id
	,footer_template_id
	,database_name
	,stored_procedure_name
	)
SELECT email_template_rule_id
	,email_template_rule_name
	,email_template_id
	,footer_template_id
	,database_name
	,stored_procedure_name
FROM $(ExcelsiorDB)..EML_EmailTemplateRule

SET IDENTITY_INSERT dbo.TBL_EML_EmailTemplateRule OFF
-- TBL_EML_EmailTemplateRuleAttribute script
SET IDENTITY_INSERT dbo.TBL_EML_EmailTemplateRuleAttribute ON

INSERT INTO TBL_EML_EmailTemplateRuleAttribute (
	email_template_rule_attribute_id
	,email_template_rule_id
	,attribute_name
	,column_name
	,datatype
	,data_mask
	,constant
	)
SELECT email_template_rule_attribute_id
	,email_template_rule_id
	,attribute_name
	,column_name
	,datatype
	,data_mask
	,constant
FROM $(ExcelsiorDB)..EML_EmailTemplateRuleAttribute

SET IDENTITY_INSERT dbo.TBL_EML_EmailTemplateRuleAttribute OFF
