import * as React from "react";
import { Stack, Label } from "office-ui-fabric-react";
import {
  DefaultButton,
  DetailsList,
  DetailsListLayoutMode,
  FontIcon,
  IColumn,
  IIconProps,
  ImageFit,
  ImageIcon,
  mergeStyles,
  mergeStyleSets,
  Persona,
  PersonaSize,
  PrimaryButton,
  SelectionMode,
  Text,
} from "@fluentui/react";
import { Buyer } from "../Modal/Buyers";
import {
  IMarkPaidRequest,
  IParticipancyFullDetails,
} from "./PaymentTable.interface";
import { useParams } from "react-router";
import { useAuth0 } from "@auth0/auth0-react";
import { markPayment } from "../Services/BidsControllerService";

const iconClass = mergeStyles({
  height: 30,
  width: 30,
});

const classNames = mergeStyleSets({
  greenYellow: [{ color: "greenyellow", fontSize: 25 }, iconClass],
  red: [{ color: "indianRed", fontSize: 22.5 }, iconClass],
});

const payIcon: IIconProps = {
  iconName: "Money",
  styles: { root: { fontSize: "2rem" } },
};

interface IPaymentsTableProps {
  isChosenSupplier: boolean;
  participancyList: Partial<IParticipancyFullDetails>[];
}

export const PaymentsTable: React.FunctionComponent<IPaymentsTableProps> = ({
  isChosenSupplier,
  participancyList,
}) => {
  const { id } = useParams<{ id: string }>();
  const { getAccessTokenSilently } = useAuth0();
  const [paymentParticipancyList, setPaymentParticipancyList] = React.useState<
    Partial<IParticipancyFullDetails>[]
  >(participancyList);
  const onClickEditPaymentStatusButton = (buyerId: string) => {
    const newPaymentParticipancyList = paymentParticipancyList.map(
      (participancyFullDetails) => {
        if (participancyFullDetails.buyerId === buyerId) {
          const url = `/${id}/markPayment`;
          const markPaidRequest: IMarkPaidRequest = {
            bidId: id,
            buyerId: participancyFullDetails.buyerId,
            hasPaid: !participancyFullDetails.hasPaid,
          };
          markPayment(markPaidRequest, url, getAccessTokenSilently);
          participancyFullDetails.hasPaid = !participancyFullDetails.hasPaid;
          return participancyFullDetails;
        } else {
          return participancyFullDetails;
        }
      }
    );
    setPaymentParticipancyList(newPaymentParticipancyList);
  };
  const fullDetailsColumns: IColumn[] = [
    {
      key: "column4",
      name: "Address",
      fieldName: "Address",
      minWidth: 80,
      maxWidth: 350,
      isRowHeader: true,
      isResizable: true,
      data: "string",
      isPadded: true,
      onRender: (bidParticipant: Partial<IParticipancyFullDetails>) => {
        return <Text>{bidParticipant.BuyerAddress}</Text>;
      },
    },
    {
      key: "column5",
      name: "Postal code",
      fieldName: "Postal code",
      minWidth: 70,
      maxWidth: 90,
      isResizable: true,
      data: "string",
      onRender: (bidParticipant: Partial<IParticipancyFullDetails>) => {
        return <Text>{bidParticipant.BuyerPostalCode}</Text>;
      },
      isPadded: true,
    },
    {
      key: "column6",
      name: "Phone number",
      fieldName: "Phone number",
      minWidth: 70,
      maxWidth: 90,
      isResizable: true,
      data: "string",
      onRender: (bidParticipant: Partial<IParticipancyFullDetails>) => {
        return <Text>{bidParticipant.BuyerPhoneNumber}</Text>;
      },
      isPadded: true,
    },
    {
      key: "column7",
      name: "Edit payment status",
      fieldName: "Edit payment status",
      minWidth: 140,
      maxWidth: 120,
      isResizable: true,
      data: "string",
      onRender: (bidParticipant: Partial<IParticipancyFullDetails>) => {
        return (
          <DefaultButton
            onClick={() =>
              onClickEditPaymentStatusButton(bidParticipant.buyerId as string)
            }
            text={bidParticipant.hasPaid ? "Canceled" : "Recieved"}
          ></DefaultButton>
        );
      },
      isPadded: true,
    },
  ];
  const columns: IColumn[] = [
    {
      key: "column1",
      name: "Name",
      fieldName: "Name",
      minWidth: 210,
      maxWidth: 350,
      isRowHeader: true,
      isResizable: true,
      isSorted: true,
      isSortedDescending: false,
      sortAscendingAriaLabel: "Sorted A to Z",
      sortDescendingAriaLabel: "Sorted Z to A",
      data: "string",
      isPadded: true,
      onRender: (bidParticipant: Partial<IParticipancyFullDetails>) => {
        return (
          <Persona
            imageUrl={bidParticipant.profilePicture}
            text={bidParticipant.buyerName}
            size={PersonaSize.size40}
          />
        );
      },
    },
    {
      key: "column2",
      name: "Payed",
      fieldName: "Payed",
      minWidth: 70,
      maxWidth: 90,
      isResizable: true,
      data: "Icon",
      onRender: (bidParticipant: Partial<IParticipancyFullDetails>) => {
        if (bidParticipant.hasPaid) {
          return (
            <FontIcon iconName="Accept" className={classNames.greenYellow} />
          );
        } else {
          return (
            <FontIcon
              iconName="CalculatorMultiply"
              className={classNames.red}
            />
          );
        }
      },
      isPadded: true,
    },
    {
      key: "column3",
      name: "Units",
      fieldName: "Units",
      minWidth: 70,
      maxWidth: 90,
      isResizable: true,
      data: "string",
      onRender: (bidParticipant: Partial<IParticipancyFullDetails>) => {
        return <Text>{bidParticipant.numOfUnits}</Text>;
      },
      isPadded: true,
    },
  ];

  return (
    <Stack horizontalAlign={"center"}>
      <Stack horizontal tokens={{ childrenGap: "2rem" }}>
        <Label>Pay with paypal:</Label>
        <DefaultButton href="http://bing.com">
          <ImageIcon
            imageProps={{
              src: "https://i.ibb.co/r3SDcfy/paypal.png",
              imageFit: ImageFit.none,
              maximizeFrame: true,
              style: { width: "64px", height: "64px" },
            }}
          />
        </DefaultButton>
      </Stack>
      <DetailsList
        items={paymentParticipancyList}
        columns={
          isChosenSupplier ? [...columns, ...fullDetailsColumns] : columns
        }
        selectionMode={SelectionMode.none}
        setKey="none"
        layoutMode={DetailsListLayoutMode.justified}
        isHeaderVisible={true}
      />
    </Stack>
  );
};
