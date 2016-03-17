using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using MultiPlatfomPlatformGame.WebEditorASP.Models;
using System.IO;

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

            using (this._connexion = new MySqlConnection(this._connectionString))
            {
                this._connexion.Open();
                String commandString = "SELECT * FROM component";

                using (MySqlCommand command = new MySqlCommand(commandString, this._connexion))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
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

            ViewBag.components = collection;
            return View(new Component());
        }

        [HttpPost]
        public ActionResult PostComponent(Component cmp, HttpPostedFileBase TexturePath)
        {
            // Verify that the user selected a file
            if (TexturePath != null && TexturePath.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(TexturePath.FileName).Replace(' ', '_').ToLower();

                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/Content/img/tiles"), fileName);
                TexturePath.SaveAs(path);

                using(this._connexion = new MySqlConnection(this._connectionString))
                {
                    using(MySqlCommand command = this._connexion.CreateCommand())
                    {
                        command.CommandText = "INSERT INTO component (name, texture, physics) VALUE (?name, ?texture, ?physics)";
                        command.Parameters.AddWithValue("name", cmp.Name);
                        command.Parameters.AddWithValue("texture", "/img/tiles/"+ fileName);
                        command.Parameters.AddWithValue("physics", cmp.Physics.Serialize());
                        this._connexion.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }

            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
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

            return Json(new {
                data = new
                {
                    success = success,
                    errors = errors,
                }
            });
        }
    }


    /// <summary>
    /// Class qui modélise la requette Ajax pour l'enregistrement d'un nouveau bloc
    /// </summary>
    public class NewBlockAjax
    {
        public NewBlockAjax() { }

        public string Json_Data { get; set; }
    }

    public class NewComponent {

        public NewComponent()
        {
                
        }


    }
}