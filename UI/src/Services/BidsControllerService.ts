import { NewBidRequest } from "../Modal/GroupDetails";
import { makePostRequest } from "./ControllerUtils";

export async function submitNewGroupForm(
  bidRequest: NewBidRequest
): Promise<void> {
  const serviceUrl = BasicControllerUrl + "/api/v1/Bids";

  await makePostRequest(serviceUrl, bidRequest);
}

const BasicControllerUrl: string = "https://localhost:5001";
