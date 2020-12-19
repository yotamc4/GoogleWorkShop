import { Bid, NewBidRequest } from "../Modal/GroupDetails";
import { ProductDetails } from "../Modal/ProductDetails";
import {
  buildUrlWithQueryParams,
  makePostRequest,
  queryParamsObjectType,
} from "./ControllerUtils";

export async function submitNewGroupForm(
  bidRequest: NewBidRequest
): Promise<void> {
  const serviceUrl = BasicControllerUrl + "/api/v1/Bids";

  await makePostRequest(serviceUrl, bidRequest);
}

export async function getBids(
  intervalNumber: number
): Promise<GetBidsResponse> {
  const serviceUrl = BasicControllerUrl + "/api/v1/Bids";
  const queryParm: queryParamsObjectType = {
    Page: intervalNumber.toString(),
  };
  const options = {
    method: "GET",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json;",
    },
  };

  const response: Response = await fetch(
    buildUrlWithQueryParams(serviceUrl, queryParm),
    options
  );

  const getBidsResponse: GetBidsResponse = (await response.json()) as GetBidsResponse;

  for (let i = 0; i < getBidsResponse.bidsPage.length; i++) {
    getBidsResponse.bidsPage[i].creationDate = new Date(
      getBidsResponse.bidsPage[i].creationDate
    );

    getBidsResponse.bidsPage[i].expirationDate = new Date(
      getBidsResponse.bidsPage[i].expirationDate!
    );
  }

  return getBidsResponse;
}

const BasicControllerUrl: string = "https://localhost:5001";

export interface GetBidsResponse {
  pageSize: number;
  numberOfPages: number;
  pageNumber: number;
  bidsPage: Bid[];
}
