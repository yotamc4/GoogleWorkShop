import * as React from "react";
import { DetailsList, IColumn } from "office-ui-fabric-react/lib/DetailsList";
import { Stack } from "office-ui-fabric-react";
import { ActionButton } from "office-ui-fabric-react";
import { SelectionMode, ProgressIndicator } from "@fluentui/react";
import Dialog from "@material-ui/core/Dialog";
import DialogContent from "@material-ui/core/DialogContent";
import { SupplierProposalForm } from "./SupplierProposalForm";
import { SuppliersSurvey } from "./SupplierSurvey";
import { addIcon, classNames, stackStyles } from "./SupplierSectionStyles";
import {
  ISupplierProposalRequest,
  ISuppliersSectionProps,
} from "./SupplierSection.interface";
import { deleteIcon } from "./SupplierSectionStyles";
import axios from "axios";
import { useParams } from "react-router";
import { Phase } from "../../Modal/ProductDetails";
import { getDate } from "../utils";
import { PaymentsTable } from "../../PaymentTable/PaymentTable";
import { useAuth0 } from "@auth0/auth0-react";
import { deleteSupplierProposal } from "../../Services/BidsControllerService";
import configData from "../../config.json";

export interface ISuppliersListState {
  items: ISupplierProposalRequest[];
}

export let _columns: IColumn[] = [];

_columns = [
  {
    key: "column1",
    name: "Supplier name",
    fieldName: "supplierName",
    minWidth: 100,
    maxWidth: 150,
    isResizable: true,
  },
  {
    key: "column2",
    name: "Price",
    fieldName: "proposedPrice",
    minWidth: 70,
    maxWidth: 100,
    isResizable: true,
    styles: { cellTitle: { marginLeft: "1.3rem" } },
    className: classNames.fileIconCell,
  },
  {
    key: "column3",
    name: "Minimum units",
    fieldName: "minimumUnits",
    minWidth: 70,
    maxWidth: 100,
    isResizable: true,
  },
  {
    key: "column5",
    name: "Date",
    fieldName: "publishedTime",
    minWidth: 140,
    maxWidth: 140,
    isResizable: true,
    className: classNames.fileIconCellDate,
    styles: { cellTitle: { marginLeft: "0.4rem" } },
  },
  {
    key: "column6",
    name: "ProgressBar",
    fieldName: "progressBar",
    minWidth: 150,
    maxWidth: 150,
    isResizable: true,
    className: classNames.fileIconCellProgressBar,
    styles: { cellTitle: { marginLeft: "-0.8rem" } },
  },
  {
    key: "column7",
    name: "Description",
    fieldName: "description",
    minWidth: 100,
    maxWidth: 100,
    isResizable: true,
    isMultiline: true,
    className: classNames.fileIconCellDescription,
    styles: { cellTitle: { marginLeft: "0.2rem" } },
  },
];

export const SuppliersSection: React.FunctionComponent<ISuppliersSectionProps> = ({
  supplierProposalRequestList,
  numberOfParticipants,
  bidPhase,
  isUserInBid,
}) => {
  const { isAuthenticated, user, getAccessTokenSilently } = useAuth0();
  const { id } = useParams<{ id: string }>();
  const [open, setOpen] = React.useState(false);
  //TODO: we should consume it from the context!!!!!
  const [
    isAddPropposalToSupplierListClicked,
    setIsAddPropposalToSupplierListClicked,
  ] = React.useState<boolean>(isUserInBid as boolean);
  const [listItems, setListItems] = React.useState<
    ISupplierProposalRequest[] | undefined
  >(supplierProposalRequestList);

  React.useEffect(() => {
    listItems?.map((supplierProposalRequest) => {
      supplierProposalRequest["progressBar"] = (
        //TODO: how to present the minimumUnits-requestedItems avoid injection
        <ProgressIndicator
          label={
            (supplierProposalRequest.minimumUnits as number) -
              numberOfParticipants >
            0
              ? `${
                  (supplierProposalRequest.minimumUnits as number) -
                  numberOfParticipants
                } units to complete`
              : "Complete"
          }
          percentComplete={
            numberOfParticipants /
            (supplierProposalRequest.minimumUnits as number)
          }
        />
      );
      supplierProposalRequest["publishedTime"] =
        String(
          (new Date(
            supplierProposalRequest?.publishedTime as string
          ).getUTCMonth() as number) + 1
        ) +
        "/" +
        String(
          (new Date(
            supplierProposalRequest?.publishedTime as string
          ).getUTCDate() as number) + 1
        ) +
        "/" +
        String(
          (new Date(
            supplierProposalRequest?.publishedTime as string
          ).getUTCFullYear() as number) + 1
        );
    });
  }, []);

  const handleClickOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const addPropposalToSupplierList = (
    supplierProposalFormDetails: Partial<ISupplierProposalRequest>
  ): void => {
    const newSupplierProposalFormDetails: ISupplierProposalRequest = {
      ...supplierProposalFormDetails,
    };
    newSupplierProposalFormDetails["publishedTime"] = getDate(
      supplierProposalFormDetails.publishedTime
    );
    newSupplierProposalFormDetails["progressBar"] = (
      <ProgressIndicator
        label={
          (supplierProposalFormDetails.minimumUnits as number) -
            numberOfParticipants >
          0
            ? `${
                (supplierProposalFormDetails.minimumUnits as number) -
                numberOfParticipants
              } units to complete`
            : "Complete"
        }
        percentComplete={
          numberOfParticipants /
          (supplierProposalFormDetails.minimumUnits as number)
        }
      />
    );
    setOpen(false);
    setListItems((listItems) => [
      newSupplierProposalFormDetails,
      ...(listItems as ISupplierProposalRequest[]),
    ]);
    setIsAddPropposalToSupplierListClicked(true);
  };

  const deletePropposalFromSupplierList = React.useCallback(() => {
    const url = `/${id}/proposals/${user.sub}`;
    deleteSupplierProposal(url, getAccessTokenSilently);
    const newListItems = listItems?.filter(
      (proposal) => proposal.supplierId != user.sub
    );
    setListItems(newListItems);
    setIsAddPropposalToSupplierListClicked(false);
  }, [listItems, user, getAccessTokenSilently]);

  switch (bidPhase) {
    case Phase.Join:
      return (
        <Stack styles={stackStyles}>
          {isAuthenticated && user[configData.roleIdentifier] === "Supplier" ? (
            <Stack horizontal>
              <ActionButton
                iconProps={
                  isAddPropposalToSupplierListClicked ? deleteIcon : addIcon
                }
                allowDisabledFocus
                disabled={false}
                checked={false}
                text={
                  isAddPropposalToSupplierListClicked
                    ? "Delete your supplier proposal"
                    : "Add a new Supplier proposal"
                }
                onClick={
                  isAddPropposalToSupplierListClicked
                    ? deletePropposalFromSupplierList
                    : handleClickOpen
                }
              ></ActionButton>
            </Stack>
          ) : (
            <></>
          )}
          <Dialog
            open={open}
            onClose={handleClose}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
          >
            <DialogContent>
              <SupplierProposalForm
                addPropposalToSupplierList={addPropposalToSupplierList}
                handleClose={handleClose}
              />
            </DialogContent>
          </Dialog>
          <></>
          <DetailsList
            items={listItems as ISupplierProposalRequest[]}
            columns={_columns}
            selectionMode={SelectionMode.none}
          />
        </Stack>
      );
    case Phase.Vote:
      return (
        <Stack>
          <SuppliersSurvey
            supliersNames={listItems?.map((supplierProposal) => {
              return {
                key: String(supplierProposal.supplierId),
                text: supplierProposal.supplierName as string,
              };
            })}
          />
        </Stack>
      );
    case Phase.Payment:
      return (
        <PaymentsTable payers={[{ name: "ofek", imageUrl: "f", paid: true }]} />
      );
    default:
      return null;
  }
};
