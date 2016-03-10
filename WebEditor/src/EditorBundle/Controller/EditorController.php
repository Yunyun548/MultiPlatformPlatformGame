<?php

namespace EditorBundle\Controller;

use Symfony\Bundle\FrameworkBundle\Controller\Controller;
use Symfony\Component\HttpFoundation\Request;
use Symfony\Component\HttpFoundation\JsonResponse;
use Symfony\Component\HttpFoundation\Response;
use Symfony\Component\Serializer\Serializer;
use Symfony\Component\Serializer\Encoder\JsonEncoder;
use Symfony\Component\Serializer\Normalizer\ObjectNormalizer;
use EditorBundle\Entity\Bloc;
use EditorBundle\Entity\Component;


class EditorController extends Controller
{
    public function indexAction()
    {
        $em = $this->get('doctrine.orm.entity_manager');
        $components = $em->getRepository('EditorBundle:Component')->findAll();
        return $this->render('EditorBundle:Default:index.html.twig', array('components' => $components));
    }

    /* Save a bloc
     * Parameters
     * Array Components
     * String Name 
     */
    public function persistJsonAction(Request $request)
    {
        $em = $this->get('doctrine.orm.entity_manager');
        $json_data = $request->request->get('json_data');

        if (!is_null($json_data)) {

            $encoders = array(new JsonEncoder());
            $normalizers = array(new ObjectNormalizer());
            $serializer = new Serializer($normalizers, $encoders);

            $bloc = $serializer->deserialize($json_data,'\EditorBundle\Entity\Bloc','json');

            $em->persist($bloc);
            $em->flush();

            // $zcel = $serializer->deserialize($bloc->getComponents(),'\EditorBundle\Entity\Bloc','json');

            $components = array();
            foreach (json_decode(json_encode($bloc->unserializeComponents()), true) as $component) {
                $imagePath = file_get_contents($this->get('kernel')->getRootDir() . '/../web' . $component["texture"]);
                $base64 = base64_encode($imagePath);

                $c = array('id' => $component["id"],
                 'name' => $component["name"],
                 'texture' => $base64,
                 'physics' => $component["physics"]);

                array_push($components, $c);
            }

            $blocJson = array('id' => $bloc->getId(),
             'name' => $bloc->getName(),
             'components' => $components);

            $a = array('status' => 'ok', 'bloc' => $blocJson);
            $json = json_encode($a);
            return new JsonResponse($json);
        }

        else {
            $a = array('status' => 'error', 'message' => 'Bad Parameters');
            $json = json_encode($a);
            return new JsonResponse($json);
        }
    }

    /* Save a new Component
     * Parameters
     * image : File image
     * name : String
     * physics : Array
    */
    public function saveComponentAction(Request $request) {

        $em = $this->get('doctrine.orm.entity_manager');
        $Name = $request->request->get('name');
        $jsonPhysics = json_encode($request->request->get('physics'));

        $filepath = $this->get('kernel')->getRootDir() . '/../web/img/tiles/' . $Name . '.png';
        $imageData=$_FILES['img'];

        if (!is_null($Name) && !is_null($jsonPhysics) && !is_null($imageData)) {

            if (move_uploaded_file($_FILES['img']['tmp_name'], $filepath)) {
                $a = array('status' => 'ok', 'message' => 'Composant enregistré !');
            }
            else {
                $a = array('status' => 'error', 'message' => 'Impossible d\'enregistrer l\'image');
            }

            $component = new Component;
            $component->setName($Name);
            $component->setTexturePath('/img/tiles/' . $Name . ".png");
            $physics = array('solid' => true, 'destructible' =>false);
            $component->setPhysics($physics);

            $em->persist($component);
            $em->flush();

            $json = json_encode($a);
            return new JsonResponse($json);
        }

        else {
            $a = array('status' => 'error', 'message' => 'Bad Parameters');
            $json = json_encode($a);
            return new JsonResponse($json);
        }

    }
}
