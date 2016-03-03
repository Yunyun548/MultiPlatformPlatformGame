using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace MultiPlatfomPlatformGame.WebEditorASP.Controllers
{
    public class EditorController : Controller
    {

        private MySqlConnection _connexion;
        private String _connectionString = "SERVER=vps2.lewebfrancais.fr; DATABASE=webeditor; UID=webeditor; PASSWORD=QUxUx5WXDp6XHr9J";

        // GET: Editor
        public ActionResult Index()
        {
            IList<Models.Component> collection = new List<Models.Component>();

            using(this._connexion = new MySqlConnection(this._connectionString))
            {
                this._connexion.Open();
                String commandString = "SELECT * FROM component";

                using (MySqlCommand command = new MySqlCommand(commandString, this._connexion))
                {
                    using(MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            collection.Add(new Models.Component(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3)
                            ));
                        }
                    }
                }
            }

            return View(collection);
        }

        public JsonResult PersistJson()
        {
            return new JsonResult();
        }
    }
}