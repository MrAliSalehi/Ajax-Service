import {AjaxService} from "./AjaxService";
import {AnotherDto, Left, OneOf, Right} from "./Models";

let controller = new AjaxService.HomeController();

async function x() {

    let result = await controller.Endpoint_1Async();
    if (result.IsRight()) {
        console.log(result.Value);
        return;
    }
    console.log(result.Error);


}

