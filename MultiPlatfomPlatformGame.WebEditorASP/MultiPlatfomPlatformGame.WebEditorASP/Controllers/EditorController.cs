using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using MultiPlatfomPlatformGame.WebEditorASP.Models;

namespace MultiPlatfomPlatformGame.WebEditorASP.Controllers
{
    public class EditorController : Controller
    {

        private MySqlConnection _connexion;
        private String _connectionString = "SERVER=vps2.lewebfrancais.fr; DATABASE=webeditor; UID=webeditor; PASSWORD=QUxUx5WXDp6XHr9J";

        // GET: Editor
        public ActionResult Index()
        {
            IList<Component> collection = new List<Component>();

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
                            collection.Add(new Component(
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

        [HttpPost]
        public JsonResult PersistJson(NewBlockAjax data)
        {
            bool success = true;
            string errors = String.Empty;
            try
            {
                // Deserialisation de l'objet
                Block newBlock = new Block().BlockDeserilizer(data.Json_Data);

                using (this._connexion = new MySqlConnection(this._connectionString))
                {
                    using (MySqlCommand commande = _connexion.CreateCommand())
                    {
                        // Generation de la requete
                        commande.CommandText = "INSERT INTO bloc (name, components) VALUE (?name, ?components)";
                        commande.Parameters.AddWithValue("?name", newBlock.Name);
                        commande.Parameters.AddWithValue("?components", newBlock.Components);

                        //Envois
                        _connexion.Open();
                        commande.ExecuteNonQuery();
                        _connexion.Clone();
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                errors = ex.Message;
            }

            return Json(new
            {
                data = new
                {
                    success = success,
                    errors = errors,
                }
            });
        }
    }


    public class NewBlockAjax
    {
        public NewBlockAjax()
        {

        }

        public string Json_Data { get; set; }
    }
}