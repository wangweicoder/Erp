using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public  class Sys_Menu
    {
        private string SQLConString = System.Configuration.ConfigurationManager.AppSettings["SQLConString"];

        /// <summary>
        /// 登陆查询权限获得对应菜单
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<Model.Menu> GetMenuList(int UserId) 
        {
            const string sql =
@"select * from Menu where  charindex(','+cast(id as varchar)+',' , ','+(select  MenuIdList from  RoleInfo where RoleCode=
(select RoleCode from UserAdmin where id=@UserId))+',')>0 ";
            return Factory.DBHelper.Query<Model.Menu>(SQLConString, sql.ToString(), new DynamicParameters(new { UserId }));
        }


        /// 分页获得数据信息
        /// </summary>
        /// <param name="limit">页码大小</param>
        /// <param name="offset">第几页</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Role">角色ID</param>
        /// <returns></returns>
        public List<Model.Menu> MenuList(int limit, int offset, string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( SELECT ROW_NUMBER() over(order by id desc) as rn ,* FROM Menu");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            strSql.Append(")T where t.rn between   @offset and (@offset+9)");
            return Factory.DBHelper.Query<Model.Menu>(SQLConString, strSql.ToString(), new DynamicParameters(new { offset }));
        }

        /// <summary>
        /// 获得总记录数
        /// </summary>
        /// <param name="StrWhere"></param>
        /// <returns></returns>
        public int GetMenuListCount(string StrWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(id) as id FROM Menu");
            if (!string.IsNullOrEmpty(StrWhere))
            {
                strSql.Append(" where  1=1 " + StrWhere);
            }
            List<Model.Menu> MenuList = Factory.DBHelper.Query<Model.Menu>(SQLConString, strSql.ToString(), new DynamicParameters(new { StrWhere }));
            return MenuList.Count() > 0 ? MenuList[0].id : 0;
        }
        /// <summary>
        /// 根据层级ID获得对应层级菜单
        /// </summary>
        /// <param name="SuperiorMenuID"></param>
        /// <returns></returns>
        public List<Model.Menu> GetMenuListByHierarchy(string Hierarchy)
        {
            const string sql =
@"SELECT * FROM MENU  WHERE Hierarchy=@Hierarchy";
            return Factory.DBHelper.Query<Model.Menu>(SQLConString, sql.ToString(), new DynamicParameters(new { Hierarchy }));
        }
        /// <summary>
        /// 通过ID获得对应信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Model.Menu GetMenuInfoById(string id) 
        {
            const string sql = 
@"SELECT * FROM MENU  WHERE id=@id";
            List<Model.Menu> MenuList = Factory.DBHelper.Query<Model.Menu>(SQLConString, sql.ToString(), new DynamicParameters(new { id }));
            return MenuList.Count() > 0 ? MenuList[0] : null;
        }

        public bool InsertMenu(Model.Menu Menu) 
        {
            const string sql =
@"INSERT INTO Menu(MenuCode,MenuName,UrlPath,OrderBy,Hierarchy,SuperiorMenuID,Terminal) VALUES(@MenuCode,@MenuName,@UrlPath,@OrderBy,@Hierarchy,@SuperiorMenuID,@Terminal)";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                Menu.MenuCode,
                Menu.MenuName,
                Menu.UrlPath,
                Menu.OrderBy,
                Menu.Hierarchy,
                Menu.SuperiorMenuID,
                Menu.Terminal
            }));
        }

        public bool EditMenu(Model.Menu Menu) 
        {
            const string sql =
@"UPDATE  Menu SET MenuCode=@MenuCode,MenuName=@MenuName,UrlPath=@UrlPath,OrderBy=@OrderBy,Hierarchy=@Hierarchy,SuperiorMenuID=@SuperiorMenuID,Terminal=@Terminal WHERE ID=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                Menu.MenuCode,
                Menu.MenuName,
                Menu.UrlPath,
                Menu.OrderBy,
                Menu.Hierarchy,
                Menu.SuperiorMenuID,    Menu.Terminal,
                Menu.id
            }));
        }

        public bool DeleteMenu(string id)
        {
            const string sql =
@"DELETE FROM Menu WHERE id=@id";
            return Factory.DBHelper.ExecSQL(SQLConString, sql.ToString(), new DynamicParameters(new
            {
                id
            }));
        }
    }
}
