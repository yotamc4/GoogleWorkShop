import { Link, Stack, StackItem, Text } from "@fluentui/react";
import * as React from "react";
import { genericGapStackTokensString } from "./ProductCardGrid/ProductCardGridStyles";

export const AboutUs: React.FunctionComponent = () => {
  return (
    <Stack
      tokens={{
        childrenGap: "2rem",
        padding: 10,
      }}
    >
      <StackItem align="center">
        <Text variant={"xxLarge"} styles={getStylesColour("#70706c")}>
          About us
        </Text>
      </StackItem>
      <StackItem align="center">
        <Text styles={getStylesColour("#70706c")}>
          We are not a shopping site. We are something new and different.
        </Text>
      </StackItem>
      <Stack tokens={genericGapStackTokensString("1rem")} styles={marginsStyle}>
        <Text block variant={"xLarge"} styles={getStylesColour("#1469d9")}>
          What is UniBuy?
        </Text>
        <Text block variant={"large"} styles={getStylesColour("#70706c")}>
          For consumers:
        </Text>
        <Text block>
          We are a platform that connects people who have the same products on
          their wish list, allows them to unite into a purchasing group, creates
          order in the supplier search process and centralizes all the details
          throughout the group purchasing process.
        </Text>
        <Text block styles={getStylesColour("#334b6b")}>
          In short, we’re a new alternative for you to get sweet deals on your
          desired products.
        </Text>
        <Text block variant={"large"} styles={getStylesColour("#70706c")}>
          For suppliers:
        </Text>
        <Text block>
          We unite consumers into groups who are willing to purchase in bulk,
          allow suppliers to browse these groups and make them proposals, and
          centralizes all details throughout the selling process in case their
          proposals are selected. In short, we are a new and convenient way for
          you to wholesale.
        </Text>
        <Text block styles={getStylesColour("#334b6b")}>
          In short, we are a new and convenient way for you to wholesale.
        </Text>
      </Stack>
      <Stack tokens={genericGapStackTokensString("1rem")} styles={marginsStyle}>
        <Text block variant={"xLarge"} styles={getStylesColour("#1469d9")}>
          How it all works:
        </Text>
        <Text block>
          Consumers can join existing group-buys and create new ones from
          scratch. A group-buy is characterized by a requested product, an
          expiration date and a maximal price. Suppliers can make proposals for
          the various groups (if they can provide the requested product for
          at-most the group’s maximal price), an offer is characterized by a
          price and minimal quantity of units.
        </Text>
        <Text block variant={"large"} styles={getStylesColour("#70706c")}>
          When the group's expiration date arrives:
        </Text>
        <Text block styles={extraMarginsStyle}>
          <li>
            If there are no relevant proposals to the group, the group closes.
          </li>
          <li>
            If there is only one relevant proposal, it is automatically
            selected, and the group is transferred to the payment stage.
          </li>
          <li>
            If there are several relevant proposals for the group, a survey is
            conducted between the group members for 48 hours, at the end of this
            time window one proposal is selected (by a majority of votes) and
            the group is transferred to the payment stage.
          </li>
        </Text>
        <Text block variant={"large"} styles={getStylesColour("#70706c")}>
          Payment:
        </Text>
        <Text block>
          The selected proposal’s supplier provides a{" "}
          <Link href="https://www.paypal.com/us/smarthelp/article/faq3025">
            PayPal.Me{" "}
          </Link>{" "}
          link and the group members pay using PayPal, this assures security in
          payments and provides a dispute system for consumers. When the group
          is at the payment stage, the group’s page a table indicating the
          payment status of each participant. After all participants have paid
          the supplier ships the product to them and closes the group with
          completion of the deal.
        </Text>
      </Stack>
    </Stack>
  );
};

const marginsStyle = { root: { marginLeft: "15rem", marginRight: "15rem" } };
const extraMarginsStyle = {
  root: { marginLeft: "2rem" },
};

const getStylesColour = (colorId: string) => {
  return {
    root: {
      color: colorId,
    },
  };
};
